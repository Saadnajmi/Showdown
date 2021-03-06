import logging

from django.conf import settings

from rest_framework.views import APIView
from rest_framework.response import Response

from admin import userlist 

import jwt

from util import facebook

from .models import User

logger = logging.getLogger('showdown.%s' % __name__)

def build_claim(user):
    """Builds the payload for the JSON Web Token"""
    return {
        'sub': str(user.id),
        'iss': 'http://texas-msa.org',
        'permission': 'admin' if user.adminstrator else '',
        'meta': {
            'name': user.name
        }
    }

class LoginView(APIView):
    def post(self, request, format=None):
        try:
            facebook_access_token = request.data['facebookAccessToken']
        except KeyError:
            return Response(status=400)

        try:
            user_id, name = facebook.get_token_info(facebook_access_token)
            user, _= User.objects.get_or_create(facebook_id=user_id, name=name, adminstrator=True if name in userlist.user_list else False)
            claim = build_claim(user)
            jwt_token = jwt.encode(claim, settings.SECRET_KEY).decode("utf-8")
            return Response({'token': jwt_token}, status=200)
        except Exception as e:
            logger.exception(e)
            return Response(status=401)
