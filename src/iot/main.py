# GREY BUTTON - Tare
# YELLOW BUTTON - Change scale
# GREEEN BUTTON - Change working session
# BLUE BUTTON - SEND DATA ON SERVER (imitation)


import network
import time
from utime import sleep_us, sleep_ms
from machine import I2C, Pin, freq
import dht
try:
    import urequests as requests
except ImportError:
    import requests
import ujson
from scales import Scales
from i2c_lcd import I2cLcd 

SERIAL_NUMBER="20230615001"
METRIC_SCALE = 0.42
IMPERIAL_SCALE = METRIC_SCALE / 0.035274
HX_SCK_PIN = 2
HX_DT_PIN = 4
DT_PIN = Pin(4)
TARE_PIN = Pin(13, Pin.IN, Pin.PULL_UP)
CHANGE_SCALE_PIN = Pin(12, Pin.IN, Pin.PULL_UP)
NEXT_PIN = Pin(14, Pin.IN, Pin.PULL_UP)
SEND_PIN = Pin(27, Pin.IN, Pin.PULL_UP)
WORKING_SESSIONS = []
CURRENT_SESSION = 0

print(f"current frequency: {freq() / 10 ** 6} MHz")

def request_sessions():
    request = open("responseExample.json")
    requstJson = ujson.loads(request.read())
    sessions = requstJson["data"];

    for session in sessions:
        if (session["end"] is not None) and (session["measurement"] is None):
            WORKING_SESSIONS.append({
                "id": session["id"],
                "hash": session["hash"]
            })
    
    request.close()


def setup():
    global hx, lcd

    configFile = open("config.json", "r")
    config = ujson.loads(configFile.read())
    sta_if = network.WLAN(network.STA_IF)
    sta_if.active(True)
    sta_if.connect(config["WlanName"], config["WlanPassword"])
    while not sta_if.isconnected():
        time.sleep(0.1)
    print("Wifi Connected!")
    configFile.close()

    request_sessions()

    hx = Scales(HX_DT_PIN, HX_SCK_PIN)
    hx.set_scale(IMPERIAL_SCALE)
    hx.tare()

    AddressOfLcd = 0x27
    i2c = I2C(scl=Pin(22), sda=Pin(21), freq=400000)
    lcd = I2cLcd(i2c, AddressOfLcd, 2, 16)

    lcd.move_to(13, 0)
    lcd.putstr('0.0')

    lcd.move_to(0, 1)
    lcd.putstr('SID:')
    if(len(WORKING_SESSIONS) > 0):
        lcd.putstr(WORKING_SESSIONS[CURRENT_SESSION]["hash"])
    else:
        lcd.putstr('OUT!   ')

    lcd.move_to(14, 1)
    lcd.putstr('oz')   

def write(s, row):
    row_length = 16
    to_print = s[:min(len(s), row_length)]
    to_print = (' ' * (row_length - len(to_print))) + to_print
    lcd.move_to(0, row)
    lcd.putstr(to_print)

setup()

di = {}
j = 0
while True:
    if (TARE_PIN.value() == 0):
        hx.tare()

    elif (CHANGE_SCALE_PIN() == 0):
        if (hx.scale == METRIC_SCALE):
            hx.set_scale(IMPERIAL_SCALE)
            lcd.move_to(14, 1)
            lcd.putstr('oz')  
        else:
            hx.set_scale(METRIC_SCALE)
            lcd.move_to(14, 1)
            lcd.putstr(' g')

    elif (NEXT_PIN.value() == 0):
        if(len(WORKING_SESSIONS) > 0):
            CURRENT_SESSION = (CURRENT_SESSION + 1) % len(WORKING_SESSIONS)
            lcd.move_to(4, 1)
            lcd.putstr(WORKING_SESSIONS[CURRENT_SESSION]["hash"])
        else:
            lcd.move_to(4, 1)
            lcd.putstr('OUT!   ')
    
    elif (SEND_PIN.value() == 0):
        if(len(WORKING_SESSIONS) > 0):
            lcd.move_to(0, 1)
            lcd.putstr('sending...')
            sleep_ms(2000)
            
            print('Data was sended')
            
            lcd.move_to(0, 1)
            lcd.putstr('sended!   ')
            sleep_ms(2000)

            WORKING_SESSIONS.pop(CURRENT_SESSION)
            if (len(WORKING_SESSIONS) == 0):
                lcd.move_to(0, 1)
                lcd.putstr('OUT!   ')
            else:
                CURRENT_SESSION = (CURRENT_SESSION + 1) % len(WORKING_SESSIONS)
                lcd.move_to(0, 1)
                lcd.putstr('SID:' + WORKING_SESSIONS[CURRENT_SESSION]["hash"])
        else:
            print('Data was NOT sended')
            lcd.move_to(4, 1)
            lcd.putstr('OUT!   ')


    val = hx.get_units()
    if val not in di.keys():
        di[val] = 1
    else:
        di[val] += 1
    j += 1
    
    if j > 4:
        max_key = -1
        max_value = -1
        for key in di.keys():
            if (di[key] > max_value):
                max_key = key
                max_value = di[key]
        write(str(max_key), 0)
        di = {}
        j = 0

    sleep_us(100000)