import {Routes} from '@angular/router';
import {ElectionComponent} from "./election/election.component";
import {HomeComponent} from "./home/home.component";
import {NotFoundComponent} from "./core/components/not-found/not-found.component";
import {ServerErrorComponent} from "./core/components/server-error/server-error.component";

export const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'not-found', component: NotFoundComponent},

  {path: "election/:roomCode", component: ElectionComponent},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];
