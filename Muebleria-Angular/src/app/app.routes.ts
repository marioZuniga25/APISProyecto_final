import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { GraciasComponent } from './gracias/gracias.component';
import { EnvioComponent } from './envio/envio.component';
import { DetalleComponent } from './detalle/detalle.component';
import { ContactoComponent } from './contacto/contacto.component';
import { CatalogoComponent } from './catalogo/catalogo.component';
import { BagComponent } from './bag/bag.component';
import { LoginComponent } from './Admin/login/login.component';
import { DashboardComponent } from './Admin/dashboard/dashboard.component';
import { ProveedorComponent } from './Admin/proveedor/proveedor.component';
import { MateriaPrimaComponent } from './Admin/materia-prima/materia-prima.component';
import { VentaComponent } from './Admin/venta/venta.component';
import { UsuariosComponent } from './Admin/usuarios/usuarios.component';
import { RegisterComponent } from './Usuario/register/register.component';
import { LoginUsuarioComponent } from './Usuario/login-usuario/login-usuario.component';
import { InicioAdminComponent } from './Admin/inicio-admin/inicio-admin.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'gracias',
    component: GraciasComponent,
  },
  {
    path: 'envio',
    component: EnvioComponent,
  },
  {
    path: 'detalle',
    component: DetalleComponent,
  },
  {
    path: 'contacto',
    component: ContactoComponent,
  },
  {
    path: 'catalogo',
    component: CatalogoComponent,
  },
  {
    path: 'bag',
    component: BagComponent,
  },
  {
    path: 'admin',
    component: LoginComponent,
  },
  {
    path: 'admin/dashboard',
    component: DashboardComponent,
  },
  {
    path: 'admin/proveedor',
    component: ProveedorComponent,
  },
  {
    path: 'admin/materia_prima',
    component: MateriaPrimaComponent,
  },
  {
    path: 'admin/venta',
    component: VentaComponent,
  },
  {
    path: 'admin/usuarios',
    component: UsuariosComponent,
  },
  {
    path: 'admin/inicio',
    component: InicioAdminComponent,
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'login',
    component: LoginUsuarioComponent,
  },
];
