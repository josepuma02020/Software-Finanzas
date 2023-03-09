import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { ResponseStatus } from '../interceptors/commom/commom';
import { MatDialog } from '@angular/material/dialog';
import { ShowErrorModalComponent } from '../Shared/show-error-modal/show-error-modal/show-error-modal.component';
@Injectable({
  providedIn: 'root'
})
export class Service {
  private responseStatus: ResponseStatus = new ResponseStatus();
  constructor(protected httpClient: HttpClient, protected dialog: MatDialog) {

  } protected ejecutarVerboPut<T>(request: any, controller: string): Observable<T> {
    return this.httpClient.put<T>(controller, request).pipe(catchError(this.transformError.bind(this)));
  }
  protected ejecutarVerboPost<T>(request: any, controller: string): Observable<T> {
    return this.httpClient.post<T>(controller, request).pipe(catchError(this.transformError.bind(this)));
  }
  protected ejecutarVerboGet<T>(controller: string): Observable<T> {
    return this.httpClient.get<T>(controller).pipe(catchError(this.transformError.bind(this)));
  }
  public transformError(error: HttpErrorResponse | string) {
    this.responseStatus = new ResponseStatus();;
    if (typeof error === 'string') {
      this.responseStatus.titulo = error;
      this.responseStatus.classAlert = 'accent';
      this.mostrarError(this.responseStatus);
    } else if (error instanceof HttpErrorResponse) {
      if (typeof error.error !== 'object') {
        this.responseStatus.titulo = error.error;
      }
      this.responseStatus.errors = [];
      if (error.status === 400) {
        this.responseStatus.titulo = error.error.Message;
        if (error.error.Errors) {
          error.error.Errors.forEach((element: any) => {
            this.responseStatus.errors?.push(element.ErrorMessage);
          });
        }
        this.responseStatus.classAlert = 'accent';
        this.mostrarError(this.responseStatus);
      } else {
        this.responseStatus.classAlert = 'warn';
        if (error.status === 403) {
          this.responseStatus.titulo = 'No tiene permitido usar este recurso.';
          this.mostrarError(this.responseStatus);
        } else {
          if (error.status === 500) {
            this.responseStatus.titulo = error.error.Message;
            this.mostrarError(this.responseStatus);
          }
        }
      }
    }

    return throwError(this.responseStatus);
  }
  private mostrarError(response: ResponseStatus): void {
    if (response.errors || response.titulo) {

      this.dialog.open(ShowErrorModalComponent, {
        panelClass: 'modal-response',
        width: '30%',
        data: response
      });

    }
  }
}
