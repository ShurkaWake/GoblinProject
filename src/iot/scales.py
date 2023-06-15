from hx711 import HX711
from utime import sleep_us, sleep_ms


class Scales(HX711):
    def __init__(self, d_out, pd_sck):
        super(Scales, self).__init__(d_out, pd_sck)
        self.offset = 0
        self.scale = 1
        sleep_ms(50)

    def reset(self):
        self.power_off()
        self.power_on()

    def tare(self):
        self.offset = self.stable_value()

    def raw_value(self):
        return self.read()

    def stable_value(self, reads=5, delay_us=50000):
        values = []
        for _ in range(reads):
            values.append(self.raw_value())
            sleep_us(delay_us)

        di = {}
        for val in values:
            if val not in di.keys():
                di[val] = 1
            else:
                di[val] += 1

        max_key = -1
        max_value = -1
        for key in di.keys():
            if (di[key] > max_value):
                max_key = key
                max_value = di[key]
        return max_key

    def set_scale(self, scale):
        self.scale = scale

    def get_stable_units(self):
        value = self.stable_value()
        if abs(value - self.offset) < 3:
            return 0.0
        return (value - self.offset) / self.scale
    
    def get_units(self):
        value = self.raw_value()
        if abs(value - self.offset) < 3:
            return 0.0
        return (value - self.offset) / self.scale