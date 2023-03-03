import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './PaginasGenerales/nav-menu/nav-menu.component';
import { HomeComponent } from './PaginasGenerales/home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { FormularioNotaContablePorRegistroComponent } from './NotasContables/formularioNotaContablePorRegistro/formularioNotaContablePorRegistro.component';
import { notaContablePorConceptoComponent } from './NotasContables/notaContablePorConcepto/notaContablePorConcepto.component';
import { notaContablePorAdjuntoComponent } from './NotasContables/notaContablePorAdjunto/notaContablePorAdjunto.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    FormularioNotaContablePorRegistroComponent,
    notaContablePorConceptoComponent,
    notaContablePorAdjuntoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule, 
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'formularioNotaContablePorRegistro', component: FormularioNotaContablePorRegistroComponent },
      { path: 'notaContablePorConcepto', component: notaContablePorConceptoComponent },
      { path: 'notaContablePorAdjunto', component: notaContablePorAdjuntoComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
