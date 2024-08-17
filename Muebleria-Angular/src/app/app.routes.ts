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
import { AuthGuard } from './guards/auth.guard';
import { AuthGuardAdmin } from './guards/auth-admin.guard';
import { BuscadorComponent } from './buscador/buscador.component';
import { PerfilComponent } from './perfil/perfil.component';
import { ProductosComponent } from './Admin/productos/productos.component';
import { ProduccionComponent } from './Admin/produccion/produccion.component';
import { PedidosComponent } from './Admin/pedidos/pedidos.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'gracias/:id',
    component: GraciasComponent,
  },
  {
    path: 'envio',
    component: EnvioComponent,
  },
  { path: 'detalle/:id', 
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
    path: 'buscador',
    component: BuscadorComponent,
  },
  {
    path: 'admin/proveedor',
    component: ProveedorComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },

  {
    path: 'admin/pedidos',
    component: PedidosComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'admin/materiaprima',
    component: MateriaPrimaComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'admin/venta',
    component: VentaComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'admin/usuarios',
    component: UsuariosComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'admin/productos',
    component: ProductosComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'admin/produccion',
    component: ProduccionComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'admin/inicio',
    component: InicioAdminComponent,
    canActivate: [AuthGuard, AuthGuardAdmin],
  },
  {
    path: 'register',
    component: RegisterComponent,
  },
  {
    path: 'login',
    component: LoginUsuarioComponent,
  },
  {
    path: 'perfil/:id',
    component: PerfilComponent
  }
];
