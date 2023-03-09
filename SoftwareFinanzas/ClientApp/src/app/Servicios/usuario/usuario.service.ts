import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Service } from '../service';
import { catchError, map, Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { ResponseHttp } from 'src/app/models/response';
import { transformError } from 'src/app/interceptors/commom/commom';
import { UsuarioParametrizado } from 'src/app/models/usuarios/usuarioparametrizado.model';
import { UsuarioConsultaDtoModel } from 'src/app/models/usuarios/usuario-consulta-dto.model';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService extends Service {
  private ruta = document.getElementsByTagName('base')[0].href + 'api/';
  constructor(public httpClient: HttpClient, dialog: MatDialog) {
    super(httpClient, dialog)
  }
  public getUsuarios(parametros: UsuarioParametrizado): Observable<UsuarioConsultaDtoModel[]> {
    return this.httpClient.post<UsuarioConsultaDtoModel[]>(`${this.ruta}Usuario`, parametros)
  }
}
