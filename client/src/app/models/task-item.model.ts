export interface TaskItem {
  id: number;
  title: string;
  isCompleted: boolean;
  projectId: number;
}

export interface CreateTaskItemDto {
  title: string;
}

export interface UpdateTaskItemDto {
  title?: string;
  isCompleted?: boolean;
}

export interface ToggleTaskCompletionDto {
  isCompleted: boolean;
}


