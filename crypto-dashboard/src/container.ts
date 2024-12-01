import { Container } from "inversify";
import AuthService from "./services/AuthService";
import CryptoService from "./services/CryptoService";

const container = new Container();

container.bind(AuthService).toSelf().inSingletonScope();
container.bind(CryptoService).toSelf().inSingletonScope();

export default container;
