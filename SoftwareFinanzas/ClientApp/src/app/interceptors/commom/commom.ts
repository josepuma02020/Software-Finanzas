import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

export class ResponseStatus {
  constructor(public titulo?: string, public errors?: string[], public classAlert?: string) { }
}
export function transformError(error: HttpErrorResponse | string) {
  const errorDescription = new ResponseStatus();
  if (typeof error === 'string') {
    errorDescription.titulo = error;
    errorDescription.classAlert = 'warning';
  } else if (error instanceof HttpErrorResponse) {
    if (typeof error.error !== 'object') {
      errorDescription.titulo = error.error;
    }
    errorDescription.errors = [];
    if (error.status === 400) {
      if (error.error.Errors) {
        error.error.Errors.forEach((element: any) => {
          errorDescription.errors?.push(element.ErrorMessage);
        });
      }
      errorDescription.classAlert = 'warning';
    } else {
      errorDescription.titulo = error.error.Message;
      errorDescription.classAlert = 'danger';
    }
  }
  return throwError(errorDescription);
}
