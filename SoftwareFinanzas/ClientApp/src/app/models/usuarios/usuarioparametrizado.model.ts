import { Filtrorolusuario, } from "../enums/filtrorolusuario.model";

export class UsuarioParametrizado {
    public filtrorol: Filtrorolusuario = Filtrorolusuario.All;
    public filtroidentificacion: boolean = false;
    public filtroEmail: boolean = false;
    public filtroProceso: boolean = false;
    public filtroEquipo: boolean = false;
    public filtroArea: boolean = false;
    public nombre?: string = "";
    public identificacion?: string = "";
    public email?: string = "";

    constructor(
    ) {

    }

}

