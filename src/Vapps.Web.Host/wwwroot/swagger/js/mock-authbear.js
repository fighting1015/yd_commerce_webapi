﻿$(document).ready(function () {
    addAuthorization();
});

function addAuthorization() {
    var token =
        'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJBc3BOZXQuSWRlbnRpdHkuU2VjdXJpdHlTdGFtcCI6IjExYjU2YjJlLWRkYzMtNGZmMC05MDlmLTliMTVjMTdhMTU0MCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwic3ViIjoiMSIsImp0aSI6ImU0MzFhMjQ2LTA5ZDQtNDI5Ny04ODhhLTBmZWEzMGZjMDk4YiIsImlhdCI6MTQ5NDg2NTg5OCwibmJmIjoxNDk0ODY1ODk4LCJleHAiOjE0OTQ5NTIyOTgsImlzcyI6IlZhcHBzWmVybyIsImF1ZCI6IlZhcHBzWmVybyJ9.v14PfricUMWesHVBeP5vw2fd98tMYmTB6UGZ6ec1hlI';
    window.swaggerUi.api.clientAuthorizations.add("key", new SwaggerClient.ApiKeyAuthorization('Authorization', 'Bearer ' + token, 'header'));
}