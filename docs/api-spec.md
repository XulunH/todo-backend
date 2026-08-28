# To-Do API Specifications
---
This is a proposal for a 'To-Do List' API that will allow the user to manage tasks on a to-do list. They will have to ability to create new tasks as well as update or delete existing ones. 
## Endpoints
---
**Fetch All Tasks Endpoint**

`GET /tasks`

Parameters: 
    
    name: ?completed=bool
    type: bool
    description: filters tasks by completed status
     
    name: ?sort_by=+dueDate (Optional)
    type: datetime
    description: sorts by due date (ascending)

    name: ?sort_by=-dueDate (Optional)
    type: datetime
    description: sorts by due date (descending)

    name: ?sort_by=+createdDate (Optional)
    type: datetime
    description: sorts by created date (ascending)

    name: ?sort_by=-createdDate (Optional)
    type: datetime
    description: sorts by created date (descending)




Response Code: 200 OK 
Response Body:

```
[
    {
        "id": GUID,
        "taskDescription": "string",
        "createdDate": "datetime",
        "dueDate": "datetime",
        "completed": bool
    },
    {
        "id": GUID,
        "taskDescription": "string",
        "createdDate": "datetime",
        "dueDate": "datetime",
        "completed": bool
    },
    {
        "id": GUID,
        "taskDescription": "string",
        "createdDate": "datetime",
        "dueDate": "datetime",
        "completed": bool
    },
    ...
]
```

Failure States: 

4XX Client Error
```
{
    message: "string"
}
```
<br>



**Fetch Task Endpoint**

`GET /tasks/{id}`

Response Code: 200 OK
Response Body:
```

{
    "id": GUID,
    "taskDescription": "string",
    "createdDate": "datetime",
    "dueDate": "datetime",
    "completed": bool
}
```
Failure States: 

4XX Client Error
```
{
    message: "string"
}
```
<br>



**Create Task Endpoint**

`POST /tasks`

Request Body:
```
{
    "taskDescription": "string",
    "dueDate": "datetime",
    "completed": bool
}
```

Response Code: 201 CREATED
Response Body:
```
{
    "id": GUID,
    "taskDescription": "string",
    "createdDate": "datetime",
    "dueDate": "datetime",
    "completed": bool
}
```
Failure States: 

4XX Client Error
```
{
    message: "string"
}
```
<br>



**Update Task Endpoint**

`PUT /tasks/{id}`

Request Body: 
```
{
    "id": GUID,                      //inmutable
    "taskDescription": "string",
    "createdDate": "datetime",         //inmutable
    "dueDate": "datetime",
    "completed": bool
}
```

Response Code: 200 OK
Response Body:

```
{
    "id": GUID,
    "taskDescription": "string",
    "createdDate": "datetime",
    "dueDate": "datetime",
    "completed": bool
}
```
Failure States: 

4XX Client Error
```
{
    message: "string"
}
```
<br>



**Delete Task Endpoint**

`DELETE /tasks/{id}`

Repsonse Code: 200 OK
Response Body:

Failure States: 

4XX Client Error
```
{
    message: "string"
}
```
```