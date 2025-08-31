import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ProjectsService } from '../../services/projects.service';
import { TasksService } from '../../services/tasks.service';
import { Project } from '../../models/project.model';
import { TaskItem } from '../../models/task-item.model';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <section>
      <a routerLink="/">← Geri</a>
      <h2>Proje Detayı</h2>

      <div *ngIf="loading">Yükleniyor...</div>
      <div *ngIf="error" style="color:#b00020">{{ error }}</div>

      <div *ngIf="project">
        <h3>{{ project.name }}</h3>
        <p *ngIf="project.description">{{ project.description }}</p>

        <h4>Görevler</h4>
        <ul>
          <li *ngFor="let t of tasks">
            <label>
              <input type="checkbox" [checked]="t.isCompleted" (change)="onToggle(t)" />
              <span [style.textDecoration]="t.isCompleted ? 'line-through' : 'none'">{{ t.title }}</span>
            </label>
          </li>
        </ul>

        <hr />
        <h4>Yeni Görev Ekle</h4>
        <form [formGroup]="form" (ngSubmit)="onSubmit()">
          <input type="text" formControlName="title" placeholder="Görev başlığı" />
          <button type="submit" [disabled]="form.invalid || submitting" style="margin-left:8px;">
            {{ submitting ? 'Kaydediliyor...' : 'Ekle' }}
          </button>
          <div *ngIf="form.controls.title.touched && form.controls.title.invalid" style="color:#b00020; margin-top:6px;">
            Başlık zorunlu ve en az 2 karakter olmalı.
          </div>
        </form>
      </div>
    </section>
  `
})
export class ProjectDetailComponent implements OnInit {
  projectId = 0;
  project: Project | null = null;
  tasks: TaskItem[] = [];
  loading = false;
  error: string | null = null;
  submitting = false;

  form = new FormGroup({
    title: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(2)] })
  });

  constructor(private route: ActivatedRoute, private projectsService: ProjectsService, private tasksService: TasksService) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    this.projectId = idParam ? Number(idParam) : 0;
    this.loadProject();
    this.loadTasks();
  }

  loadProject(): void {
    this.loading = true;
    this.error = null;
    this.projectsService.getById(this.projectId).subscribe({
      next: (p) => {
        this.project = p;
        this.loading = false;
      },
      error: () => {
        this.error = 'Proje yüklenemedi.';
        this.loading = false;
      }
    });
  }

  loadTasks(): void {
    this.projectsService.getTasks(this.projectId).subscribe({
      next: (items) => {
        this.tasks = items;
      },
      error: () => {
        this.error = 'Görevler yüklenemedi.';
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    const payload = { title: this.form.controls.title.value } as { title: string };
    this.projectsService.addTask(this.projectId, payload).subscribe({
      next: () => {
        this.submitting = false;
        this.form.reset();
        this.loadTasks();
      },
      error: () => {
        this.submitting = false;
        this.error = 'Görev eklenemedi.';
      }
    });
  }

  onToggle(t: TaskItem): void {
    this.tasksService.toggleComplete(t.id, { isCompleted: !t.isCompleted }).subscribe({
      next: () => {
        // Basit yaklaşım: listeyi yeniden yükle
        this.loadTasks();
      },
      error: () => {
        this.error = 'Görev güncellenemedi.';
      }
    });
  }
}


