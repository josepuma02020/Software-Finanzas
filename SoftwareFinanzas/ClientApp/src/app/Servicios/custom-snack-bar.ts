import { Injectable, NgZone } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar'
@Injectable({
    providedIn: 'root'
})
export class CustomSnackBarService {

    constructor(
        public snackBar: MatSnackBar,
        private zone: NgZone
    ) {

    }

    public open(mensaje: string, accion = "operación", panelClass: string = 'green', duration = 4000) {
        return this.zone.run(() => {
            return this.snackBar.open(mensaje, accion, { duration, panelClass: [panelClass] });
        });
    }
}
