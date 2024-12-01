const APP_LOCAL_DOMAIN = "localhost";
const endpoints = {
  AUTHORIZATION: `http://${APP_LOCAL_DOMAIN}:3000/login`,
  REVOKE: `http://${APP_LOCAL_DOMAIN}:3000/logout`,
  INFO: `http://${APP_LOCAL_DOMAIN}:3000/loginInfo`,

  GRAPHQL: `http://${APP_LOCAL_DOMAIN}:3001/graphql`,
}

export default endpoints;