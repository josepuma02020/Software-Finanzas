import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../Servicios/usuario/usuario.service';
import { CustomSnackBarService } from '../Servicios/custom-snack-bar';
import { UsuarioConsultaDtoModel } from '../models/usuarios/usuario-consulta-dto.model';
import { UsuarioParametrizado } from '../models/usuarios/usuarioparametrizado.model';
import { Filtrorolusuario } from '../models/enums/filtrorolusuario.model';

@Component({
  selector: 'app-usuarios',
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.css']
})
export class UsuariosComponent implements OnInit {

  public usuarios: UsuarioConsultaDtoModel[] = []
  private _parametrosusuario: UsuarioParametrizado = new UsuarioParametrizado();


  constructor(private usuarioService: UsuarioService, private _snackBar: CustomSnackBarService) { }

  async ngOnInit() {
    this.getUsuarios();
  }
  public getUsuarios(): void {
    this.usuarioService.getUsuarios(this._parametrosusuario).subscribe((res: UsuarioConsultaDtoModel[]) => {
      this.usuarios = res;
    });
  }

}
