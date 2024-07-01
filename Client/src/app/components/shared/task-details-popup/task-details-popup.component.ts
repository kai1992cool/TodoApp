import { Component, EventEmitter, Input, Output } from '@angular/core';
import { task } from '../../../interfaces/task';
import { CommonModule } from '@angular/common';
import { patchDocument } from '../../../interfaces/patchDocument';
import { TodoService } from '../../../services/todoServices/todo.service';
import { ToastrService } from 'ngx-toastr';
import { UtilsService } from '../../../services/utilServices/utils.service';
import { componentActions } from '../../../constants/constants';

@Component({
  selector: 'app-task-details-popup',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './task-details-popup.component.html',
  styleUrl: './task-details-popup.component.scss',
})
export class TaskDetailsPopupComponent {
  @Input() task: task;
  @Input() componentAction: string;
  componentActions;
  visible: boolean = false;
  @Output() refreshTasks: EventEmitter<any>;

  constructor(
    private todoService: TodoService,
    private toaster: ToastrService,
    private utilsServices: UtilsService
  ) {
    this.task = {} as task;
    this.componentAction = '';
    this.componentActions = componentActions;
    this.refreshTasks = new EventEmitter<any>();
  }

  getRelativeTime(date: Date): string {
    const now = new Date();
    const past = new Date(date);
    const diffInSeconds = Math.floor((now.getTime() - past.getTime()) / 1000);
    const diffInMinutes = Math.floor(diffInSeconds / 60);
    const diffInHours = Math.floor(diffInMinutes / 60);
    const diffInDays = Math.floor(diffInHours / 24);

    if (diffInDays > 0) {
      return `Added ${diffInDays} day${diffInDays > 1 ? 's' : ''} ago`;
    } else if (diffInHours > 0) {
      return `Added ${diffInHours} hour${diffInHours > 1 ? 's' : ''} ago`;
    } else if (diffInMinutes > 0) {
      return `Added ${diffInMinutes} minute${diffInMinutes > 1 ? 's' : ''} ago`;
    } else {
      return `Added ${diffInSeconds} second${diffInSeconds > 1 ? 's' : ''} ago`;
    }
  }

  taskCompleted(taskId: number) {
    const patchObject: patchDocument = {
      path: 'IsCompleted',
      op: 'replace',
      value: true,
    };
    const patchObjectArray: patchDocument[] = [patchObject];
    this.todoService.completeTask(taskId, patchObjectArray).subscribe({
      next: () => {
        this.refreshTasks.emit();
        this.toaster.info('Task Completed');
      },
      error: (error) => {
        this.toaster.error(error);
      },
    });
  }

  editTask(taskId: number) {
    this.utilsServices.displayAddTaskModel.next(true);
    this.utilsServices.taskToUpdate.next(taskId);
  }

  deleteTask(taskId: number) {
    this.todoService.deleteTaskById(taskId).subscribe({
      next: () => {
        this.refreshTasks.emit(true);
        this.toaster.success('Task Deleted Successfully');
      },
      error: (error) => {
        this.toaster.error(error);
      },
    });
  }
}