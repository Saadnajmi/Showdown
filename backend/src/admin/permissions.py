from rest_framework import permissions

class AdminPermission(permissions.BasePermission):
    message = 'Must be an admin'

    def has_permission(self, request, view):
        # Disabling auth for now... DUN DUN DUN
        #return request.token and request.token['permission'] == 'admin'
        return true

class ScorekeeperPermission(permissions.BasePermission):
    message = 'Must be scorekeeper'

    def has_object_permission(self, request, view, game):
        # Disabling auth for now... DUN DUN DUN
        # return request.token and str(game.scorekeeper.pk) == request.token['sub']
        return true
