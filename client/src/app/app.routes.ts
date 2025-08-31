import { Routes } from '@angular/router';
import { ProjectListComponent } from './features/projects/project-list.component';
import { ProjectDetailComponent } from './features/projects/project-detail.component';

export const routes: Routes = [
  { path: '', component: ProjectListComponent },
  { path: 'projects/:id', component: ProjectDetailComponent },
];
