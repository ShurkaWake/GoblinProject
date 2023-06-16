export const dialogModule = {
    state: {
        message: "",
        dialog: false,
        redirectPath: ""
    },
    mutations: {
        setRedirectPath(state, path){
            state.redirectPath = path
        },
        setErrorMessage(state, message){
            state.message = message
        },
        switchDialog(state) {
            state.dialog = !state.dialog
        }
    }
}