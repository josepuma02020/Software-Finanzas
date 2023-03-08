import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';

@Component({
  selector: 'notaContablePorConcepto',
  templateUrl: './notaContablePorConcepto.component.html',
  styleUrls: ['./notaContablePorConcepto.css']
})
export class notaContablePorConceptoComponent {
  formulario: FormGroup;

  constructor() {
    this.formulario = new FormGroup({
      nombre: new FormControl('', Validators.required),
      correo: new FormControl('', [Validators.required, Validators.email]),
      departamento: new FormControl('', Validators.required),
      registro: new FormControl('', Validators.required),
      tipo: new FormControl('1', Validators.required),
      clasificacion: new FormControl('1', Validators.required),
      lista: new FormArray([])
    });
  }

  onSubmit() {
    console.log(this.formulario.value);
  }

  agregarItem() {
    const item = new FormGroup({
      nombre: new FormControl('', Validators.required)
    });
    this.lista.push(item);
  }

  eliminarItem(index: number) {
    this.lista.removeAt(index);
  }

  get lista(): FormArray {
    return this.formulario.get('lista') as FormArray;
  }

}
