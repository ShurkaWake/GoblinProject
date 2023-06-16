import axios from "axios";
import store from "@/store/index";
import router from "@/router";

export const authModule = {
    state: () => ({
        userId: "",
        userFullName: "",
        userRole: "",
        userEmail: "",
        token: "",
        isAuth: false,
        loading: false,

        loginEmail: "",
        loginPassword: "",

    }),
    mutations: {
        setUserData(state, user){
            state.userId = user.id
            state.userFullName = user.fullName
            state.userRole = user.role
            state.userEmail = user.email
            state.token = user.token
            state.isAuth = true
        },

        setAuth(state, status){
            state.isAuth = status
        },

        setLoading(state, status) {
            state.loading = status
        },

        setLoginEmail(state, email){
            state.loginEmail = email
        },

        setLoginPassword(state, password)
        {
            state.loginPassword = password
        }
    },
    actions: {
        async login({state, commit}){
            commit('setLoading', true)
            await axios.post('auth/login',
                {
                    email: state.loginEmail,
                    password: state.loginPassword
                })
                .then(response => {
                    store.commit('setUserData', response.data)
                    axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`
                    router.push("/")
                })
                .catch(response => {
                    store.commit('setErrorMessage', response.response.data.errors[0])
                    store.commit('setRedirectPath', "")
                    store.commit('switchDialog')
                })

            commit('setLoading', false)
        },

        async logout({state, commit}) {
            commit('setLoading', true)
            commit('setUserData', {
                id: "",
                fullName: "",
                role: "",
                email: "",
                token: "",
            })
            commit('setAuth', false)
            commit('setLoading', false)
        }
    }
}