const APP_PROD_DOMAIN = "hostname-dev.com";
const endpoints = {
  AUTHORIZATION: `https://auth.${APP_PROD_DOMAIN}/login`,
  REVOKE: `https://auth.${APP_PROD_DOMAIN}/logout`,
  INFO: `https://auth.${APP_PROD_DOMAIN}/loginInfo`,

  GRAPHQL: `https://crypto.${APP_PROD_DOMAIN}/graphql`,
}

export default endpoints;