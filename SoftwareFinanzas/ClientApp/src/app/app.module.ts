import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CommonModule } from '@angular/common';


import { AppComponent } from './app.component';
import { NavMenuComponent } from './PaginasGenerales/nav-menu/nav-menu.component';
import { HomeComponent } from './PaginasGenerales/home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { FormularioNotaContablePorRegistroComponent } from './NotasContables/formularioNotaContablePorRegistro/formularioNotaContablePorRegistro.component';
import { notaContablePorConceptoComponent } from './NotasContables/notaContablePorConcepto/notaContablePorConcepto.component';
import { notaContablePorAdjuntoComponent } from './NotasContables/notaContablePorAdjunto/notaContablePorAdjunto.component';
import { footerComponent } from './PaginasGenerales/footer/footer.component';
import { UsuariosComponent } from './usuarios/usuarios.component';
import { EntidadesComponent } from './Configuraciones/entidades/entidades.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ShowErrorModalComponent } from './Shared/show-error-modal/show-error-modal/show-error-modal.component';
import { MatDialog } from '@angular/material/dialog';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    FormularioNotaContablePorRegistroComponent,
    notaContablePorConceptoComponent,
    notaContablePorAdjuntoComponent,
    footerComponent, ShowErrorModalComponent, UsuariosComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule, MatToolbarModule,
    ReactiveFormsModule, MatIconModule, MatDialogModule, MatSnackBarModule, CommonModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'formularioNotaContablePorRegistro', component: FormularioNotaContablePorRegistroComponent },
      { path: 'notaContablePorConcepto', component: notaContablePorConceptoComponent },
      { path: 'notaContablePorAdjunto', component: notaContablePorAdjuntoComponent },
      { path: 'Usuarios', component: UsuariosComponent },
      { path: 'Entidades', component: EntidadesComponent },
      { path: 'footer', component: footerComponent }

    ]),
    BrowserAnimationsModule
  ],
  providers: [MatDialog],
  bootstrap: [AppComponent]
})
export class AppModule { }
