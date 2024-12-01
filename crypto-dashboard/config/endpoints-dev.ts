const APP_DEV_DOMAIN = "hostname-dev.com";
const endpoints = {
  AUTHORIZATION: `https://auth.${APP_DEV_DOMAIN}/login`,
  REVOKE: `https://auth.${APP_DEV_DOMAIN}/logout`,
  INFO: `https://auth.${APP_DEV_DOMAIN}/loginInfo`,

  GRAPHQL: `https://crypto.${APP_DEV_DOMAIN}/graphql`,
}

export default endpoints;
