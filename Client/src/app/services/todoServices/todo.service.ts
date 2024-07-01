import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment.development';
import { Observable, of } from 'rxjs';
import { tasks } from '../../interfaces/tasks';
import { task } from '../../interfaces/task';
import { patchDocument } from '../../interfaces/patchDocument';
import { BaseService } from '../BaseService/base.service';
import { addTask } from '../../interfaces/addTask';

@Injectable({
  providedIn: 'root',
})
export class TodoService extends BaseService{
  tasks = [
    {

  id: 1,
  title: 'test',
  description: 'test',
  isCompleted:false,
  createdOn: new Date(),
    },
    {

  id: 2,
  title: 'test2',
  description: 'test2',
  isCompleted:false,
  createdOn: new Date(),
    },
    {

  id: 3,
  title: 'test3',
  description: 'test3',
  isCompleted:false,
  createdOn: new Date(),
    }
  ]
  addtodoTask(task: addTask) {
    return this.http.post(`${environment.BASE_API}/ToDoItem`, task);
  }

  getallTodoTask(filters: { status: string; date: Date; }): Observable<tasks[]> {
    let params = new HttpParams().set('status', filters.status).set('date', filters.date.toISOString());
    return of(this.tasks)
    return this.http.get<tasks[]>(`${environment.BASE_API}/ToDoItem`,{params  : params});
  }

  getActiveTasks(): Observable<task[]> {
    let params = new HttpParams().set('status', false);
    return of(this.tasks) //this.http.get<task[]>(`${environment.BASE_API}/ToDoItem/TasksByStatus`,{ params: params });
  }
  
  getCompletedTasks(): Observable<task[]> {
    let params = new HttpParams().set('status', true);
    return of(this.tasks)
    return this.http.get<task[]>(`${environment.BASE_API}/ToDoItem/TasksByStatus`,{ params: params });
  }

  deleteAllTasks(filters: { status: string; date: Date; }): Observable<string> {
    let params = new HttpParams().set('status', filters.status).set('date', filters.date.toISOString());
    return this.http.delete<string>(`${environment.BASE_API}/ToDoItem`,{params : params, responseType: 'text' as 'json'});
  }

  deleteTaskById(id: number) {
    return this.http.delete(`${environment.BASE_API}/ToDoItem/${id}`);
  }

  getTaskById(id: number): Observable<task> {
    return this.http.get<task>(`${environment.BASE_API}/ToDoItem/${id}`);
  }

  updateTaskById(id: number, task: addTask) {
    return this.http.put(`${environment.BASE_API}/ToDoItem/${id}`, task);
  }

  completeTask(taskId: number, patchObjectArray: patchDocument[]): Observable<task> {
    return this.http.patch<task>(`${environment.BASE_API}/ToDoItem/${taskId}`,patchObjectArray);
  }
}
