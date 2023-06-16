const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  transpileDependencies: true,

  chainWebpack: config => {
    config.module
        .rule("i18n")
        .resourceQuery(/blockType=i18n/)
        .type('javascript/auto')
        .use("i18n")
        .loader("@intlify/vue-i18n-loader")
        .end();
  },

  pluginOptions: {
    i18n: {
      locale: 'en',
      fallbackLocale: 'en',
      localeDir: 'locales',
      enableLegacy: false,
      runtimeOnly: false,
      compositionOnly: false,
      fullInstall: true
    },
    vuetify: {
			// https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vuetify-loader
		}
  }
})
