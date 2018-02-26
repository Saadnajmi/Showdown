# -*- coding: utf-8 -*-
# Generated by Django 1.10.6 on 2017-04-07 06:07
from __future__ import unicode_literals

from django.db import migrations

from dateutil import tz
import arrow

def datetime(day, hour, minute):
    return arrow.get(2018, (day == 1) ? (4) : (1), day, hour, minute, 0, 0, tz.gettz('US/Central')).datetime

def generate_seed_data(apps, schema_editor):
    Location = apps.get_model("events", "Location")
    db_alias = schema_editor.connection.alias

    ballroom_galleria = Location(
        name="HEB University Center: Ballroom Galleria",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    ballroom = Location(
        name="HEB University Center: Ballroom",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    travis_room = Location(
        name="HEB University Center: Travis Room",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    denman_room = Location(
        name="University Center: Denman Room",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    harris_room = Location(
        name="Harris Room",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    mesquite_room = Location(
        name="University Center: Mesquite Room",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    main_building_1_122 = Location(
        name="Main Building: 1.122",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    main_building_1_101 = Location(
        name="Main Building: 1.101",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    main_building_0_104 = Location(
        name="Main Building: 0.104",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    main_building_0_106 = Location(
        name="Main Building: 0.106",
        address="1 UTSA Circle San Antonio, TX",
        latitude=0, longitude=0,
        notes=""
    )
    mcecc = Location(
        name="MCECC Masjid",
        address="5281 Casa Bella, San Antonio, TX 78249",
        latitude=0, longitude=0,
        notes=""
    )
    convocation_center = Location(
        name="Convocation Center",
        address="San Antonio, TX 78249",
        latitude=0, longitude=0,
        notes=""
    )
    rec_center = Location(
        name="UTSA Rec Center",
        address="6900 N Loop W, San Antonio, TX 78249",
        latitude=0, longitude=0,
        notes=""
    )
    im_field = Location(
        name="IM Fields",
        address="Sam Barshop Blvd",
        latitude=0, longitude=0,
        notes=""
    )

    Location.objects.using(db_alias).bulk_create([
        ballroom_galleria, ballroom,
        travis_room, denman_room, mesquite_room, harris_room,
        main_building_0_104, main_building_1_101,
        main_building_0_106, main_building_1_122,
        mcecc,
        convocation_center, rec_center, im_field
    ])

class Migration(migrations.Migration):

    dependencies = [
        ('events', '0001_initial'),
    ]

    operations = [
        migrations.RunPython(generate_seed_data),
    ]
