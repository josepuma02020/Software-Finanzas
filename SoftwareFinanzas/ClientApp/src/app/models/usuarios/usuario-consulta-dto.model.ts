export class UsuarioConsultaDtoModel {
    constructor(
        public nombreUsuario: string,
        public nombreProceso: string,
        public nombreEquipo: string,
        public nombreArea: string,
        public email: string,
        public rol: string,
        public identificacion: string,
        public nombreUsuarioAsignoProceso: string,
    ) { }
}
