import { Routes } from '@angular/router';
import { ElectionRoomComponent } from './election/election-room.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './core/components/not-found/not-found.component';
import { ServerErrorComponent } from './core/components/server-error/server-error.component';
import { RouteNames } from './core/constants/route-names';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: RouteNames.ServerError, component: ServerErrorComponent },
  { path: RouteNames.NotFound, component: NotFoundComponent },

  {
    path: `${RouteNames.ElectionRoom}/:roomCode`,
    component: ElectionRoomComponent,
  },
  { path: '**', redirectTo: 'not-found', pathMatch: 'full' },
];
