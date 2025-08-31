export interface Project {
  id: number;
  name: string;
  description?: string | null;
}

export interface CreateProjectDto {
  name: string;
  description?: string | null;
}

export interface UpdateProjectDto {
  name?: string;
  description?: string | null;
}


