import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ProjectsService } from '../../services/projects.service';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-project-list',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  template: `
    <section>
      <h2>Projeler</h2>

      <div *ngIf="loading">Yükleniyor...</div>
      <div *ngIf="error" style="color:#b00020">{{ error }}</div>

      <ul>
        <li *ngFor="let p of projects">
          <a [routerLink]="['/projects', p.id]">{{ p.name }}</a>
          <span *ngIf="p.description"> - {{ p.description }}</span>
        </li>
      </ul>

      <hr />
      <h3>Yeni Proje Ekle</h3>
      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <div>
          <label for="name">Ad</label><br />
          <input id="name" type="text" formControlName="name" />
          <div *ngIf="form.controls.name.touched && form.controls.name.invalid" style="color:#b00020">
            Ad zorunlu ve en az 3 karakter olmalı.
          </div>
        </div>
        <div style="margin-top:8px;">
          <label for="desc">Açıklama</label><br />
          <input id="desc" type="text" formControlName="description" />
        </div>
        <button type="submit" [disabled]="form.invalid || submitting" style="margin-top:12px;">
          {{ submitting ? 'Kaydediliyor...' : 'Ekle' }}
        </button>
      </form>
    </section>
  `
})
export class ProjectListComponent implements OnInit {
  projects: Project[] = [];
  loading = false;
  error: string | null = null;
  submitting = false;

  form = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.minLength(3)] }),
    description: new FormControl<string | null>('')
  });

  constructor(private readonly projectsService: ProjectsService) {}

  ngOnInit(): void {
    this.fetchProjects();
  }

  fetchProjects(): void {
    this.loading = true;
    this.error = null;
    this.projectsService.getAll().subscribe({
      next: (res) => {
        this.projects = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'Projeler yüklenemedi.';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    const payload = {
      name: this.form.controls.name.value,
      description: this.form.controls.description.value ?? null
    };
    this.projectsService.create(payload).subscribe({
      next: () => {
        this.submitting = false;
        this.form.reset();
        this.fetchProjects();
      },
      error: () => {
        this.submitting = false;
        this.error = 'Proje eklenemedi.';
      }
    });
  }
}


