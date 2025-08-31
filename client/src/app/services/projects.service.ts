import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { CreateProjectDto, Project, UpdateProjectDto } from '../models/project.model';
import { CreateTaskItemDto, TaskItem } from '../models/task-item.model';

@Injectable({ providedIn: 'root' })
export class ProjectsService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/projects`;

  getAll(): Observable<Project[]> {
    return this.http.get<Project[]>(this.baseUrl);
  }

  getById(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateProjectDto): Observable<Project> {
    return this.http.post<Project>(this.baseUrl, dto);
  }

  update(id: number, dto: UpdateProjectDto): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, dto);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  getTasks(projectId: number): Observable<TaskItem[]> {
    return this.http.get<TaskItem[]>(`${this.baseUrl}/${projectId}/tasks`);
  }

  addTask(projectId: number, dto: CreateTaskItemDto): Observable<TaskItem> {
    return this.http.post<TaskItem>(`${this.baseUrl}/${projectId}/tasks`, dto);
  }
}


