import {Routes} from '@angular/router';
import {ElectionComponent} from "./election/election.component";
import {HomeComponent} from "./home/home.component";

export const routes: Routes = [
  {path: "", component: HomeComponent},
  {path: "election/:roomCode", component: ElectionComponent},
  {path: "**", redirectTo: "", pathMatch: "full"},
];
