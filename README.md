
# AI-Powered VR Racing Game Tutorial


### Inleiding
Deze tutorial legt uit hoe je een AI-powered VR project vanaf nul kunt opzetten. Het project draait om een auto zodanig te trainen, door middel van Articifial Intelligence, dat hij in staat wordt om een complexe racebaan af te gaan. Je leert hoe je het project kunt reproduceren, van installatie tot het trainen van de AI-modellen. Aan het einde van deze tutorial kun je:
- Een AI-powered VR project opzetten.
- De benodigde software en versies installeren.
- Het verloop van de simulatie of het spel begrijpen.
- Observaties, acties en beloningen in kaart brengen.
- Objecten en hun gedragingen beschrijven.
- Resultaten van AI-training analyseren met Tensorboard.

### Installatie
- Python versie: 3.8
- TensorFlow versie: 2.6.0
- Unity versie: 2022.3.19f1

### Verloop van de Simulatie
De VR-omgeving bestaat uit een racebaan waarin de raceauto de baan moet afleggen. De ml-agent leert dit zichzelf op basis van acties en beloningen aan. De baan heeft bochten, U-bochten en barrières die hij hoort te ontwijken. Zodra hij de weg heeft afgelegd, stopt de episode.

### Observaties, Acties en Beloningen
- **Observaties:** De sensor ontvangt gegevens over de volgende checkpoint alsook de snelheid, positie van de auto.	
- **Acties:** De agent kan 6 discrete acties uitvoeren verdeeld onder twee branches.  Voor-, achteruit of niet rijden en links, rechts of niet sturen. 
- **Beloningen:** Zodra de training start begint de agent bestraffingen te krijgen (0,1). Dit is zodat de agent weet dat hij hoort te bewegen. Ook krijgt hij straffen van 0.01 wanneer hij achteruit rijdt. Hiermee zijn we zeker dat de agent vooruit rijdt, maar toch willen we de straffing kleiner houden dan niet bewegen, zodat de agent achteruit kan rijden als hij ergens vastzit. Voor het bereiken van een checkpoint krijgt de agent een beloning van 1,0. Eens hij de weg heeft afgelegd, krijgt hij een uiteindelijke beloning van 5,0.

### Beschrijving van de Objecten


### Informatie van de One-Pager

### Afwijkingen van de One-Pager


## Resultaten

### Resultaten van de training met Tensorboard


### Beschrijving van de Tensorboard Grafieken


### Opvallende Waarnemingen


## Conclusie

### Samenvatting

### Persoonlijke Visie

### Verbeteringen voor de Toekomst

## Bronvermelding


