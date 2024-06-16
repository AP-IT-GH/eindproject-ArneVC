
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
Creeër een 3D project, importeer Race car package[^1], Race kart model[^2], Racing Arch[^4] en Quick Outline[^5]. Gebruik vervolgens volgende packages.
  
| (Package) naam | Versie |
|---------------------------|-------------|
| Unity versie | 2022.3.19f1 |
| Python versie| 3.8|
|TensorFlow versie| 2.6.0 |
| ML Agents | 2.0.1 |
| OpenXR Plugin | 1.9.1 |
| XR Core Utilities | 2.3.0 |
| XR Interaction Toolkit | 2.5.4 |
| XR Legacy Input Helpers | 2.1.10 |
| XR Plugin Management | 4.4.1 |

Sleep de race kart model en race car in je scene.

### Verloop van de Simulatie
De VR-omgeving bestaat uit een racebaan waarin de raceauto de baan moet afleggen. De ml-agent leert dit zichzelf op basis van acties en beloningen aan. De baan heeft bochten, U-bochten en barrières die hij hoort te ontwijken. Zodra hij de weg heeft afgelegd, stopt de episode.

### Observaties, Acties en Beloningen
- **Observaties:** De sensor ontvangt gegevens over de volgende checkpoint alsook de snelheid, positie van de auto.	
- **Acties:** De agent kan 6 discrete acties uitvoeren verdeeld onder twee branches.  Voor-, achteruit of niet rijden en links, rechts of niet sturen. 
- **Beloningen:** Zodra de training start begint de agent bestraffingen te krijgen (0,1). Dit is zodat de agent weet dat hij hoort te bewegen. Ook krijgt hij straffen van 0.01 wanneer hij achteruit rijdt. Hiermee zijn we zeker dat de agent vooruit rijdt, maar toch willen we de straffing kleiner houden dan niet bewegen, zodat de agent achteruit kan rijden als hij ergens vastzit. Voor het bereiken van een checkpoint krijgt de agent een beloning van 1,0. Eens hij de weg heeft afgelegd, krijgt hij een uiteindelijke beloning van 5,0.

### Beschrijving van de Objecten
- **Race auto (speler)**: Dit is de speler/bestuurder die de racebaan zal voltooien via VR. Hierbij gaat de speler met snelheid door de racebaan en zal hij zo proberen de AI te verslaan.
- **Race auto (Agent)**: Dit is de AI die de racebaan zal voltooien door middel van obstakels te vermijden. Hier gaat de AI zo efficient mogelijk de track voltooien zonder al te veel obstakels te raken. Deze obstakels bestaan uit zowel de barriers als de auto van de speler.

### Informatie van de One-Pager
### Een korte samenvatting

In het VR Racing-project nemen spelers het op tegen een auto die wordt bestuurd door AI in een VR. De speler moet het racebaan de finishlijn bereiken voordat de AI dat doet. De AI wordt getraind om obstakels en barrières te vermijden en zo snel mogelijk de finish te bereiken. De AI krijgt hierbij zijn rewards als dit lukt.

Door de AI te implementeren met behulp van adversarial self-play kan deze continu verbeteren en strategieën ontwikkelen, wat de uitdaging vergroot. VR verhoogt de spelersbetrokkenheid, waardoor de game-ervaring realistischer en interactiever wordt. Door het gebruik van VR worden de spelers volledig ondergedompeld in de VR wereld en kunnen ze zich als echt formula one rijders achten.

### Afwijkingen van de One-Pager


## Resultaten

### Resultaten van de training met Tensorboard


### Beschrijving van de Tensorboard Grafieken


### Opvallende Waarnemingen


## Conclusie

### Samenvatting

### Persoonlijke Visie

### Verbeteringen voor de Toekomst

# Bronvermelding
[^1]: [Race Car package](https://assetstore.unity.com/packages/3d/vehicles/race-car-package-141690) | 3D Vehicles | Unity Asset Store. (2021, 7 augustus). Unity Asset Store.
[^2]: [Sketchfab.](https://sketchfab.com/3d-models/race-trackkarting-track-based-on-south-garda-32c21042ba144ce9bd2822a88d5b54ec) (z.d.). Race track/Karting Track based on South Garda - Download Free 3D model by Mauro3D (@maurogsw).
[^3]: [Vanmillion Studios.](https://www.youtube.com/watch?v=jr4eb4F9PSQ) (2022, 16 april). Simple car controller in Unity 3D (Part 1- movement) | Easy Unity Tutorial 2022 [Video]. YouTube.
[^4]: [Racing Arch  | 3D model.](https://www.cgtrader.com/free-3d-models/car/racing-car/racing-arch) (z.d.). CGTrader.
[^5]: [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488) (2022, 7 maart). Quick Outline world-space outline tool by Chris Nolet
[^6]: Vanmillion Studios. (2022, 16 april). Simple car controller in Unity 3D (Part 1- movement) | Easy Unity Tutorial 2022 [Video]. YouTube. https://www.youtube.com/watch?v=jr4eb4F9PSQ

