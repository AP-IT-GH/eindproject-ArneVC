# VR Racing: Competing against AI driven car

## Team

- Arne Van Campenhout
- Jarn Vaerewijck
- Hamzah Bhatti
- Youssef Kasmi

## De Logica van de VR Simulatie

In onze project gaat de speler het opnemen tegen een AI. Hierbij gaat de speler, in een VR omgeving, de auto besturen doorheen de racebaan. De speler zorgt er voor dat hij de finish behaalt voor de AI dit doet. De AI wordt getraind om zo snel mogelijk de finish te behalen en krijgt rewards als hij effectief obstacles vermijd en de finish bereikt voor de speler (human).

## De Meerwaarde van de AI-component

Om de meerwaarde van een AI component weer te kunnen geven, stellen we ons een racespel voor waarin de computergestuurde auto's geen eigen intelligentie hebben. We kunnen ze programmeren om een vooraf bepaald pad te volgen. Dit heeft het voordeel dat de auto's altijd de juiste weg afleggen. Hierdoor wordt het spel snel saai, omdat de auto's (AI) voorspelbaar zijn en de speler (human) het pad snel doorheeft, waardoor het te makkelijk wordt.

Zelfs als we randomisatie toevoegen, blijven de auto's voorspelbaar, omdat ze nog steeds alleen maar het script volgen. Hierdoor lossen we weinig op.

Door de tegenstanders een eigen intelligentie te geven en ze te trainen, kunnen ze steeds verschillende beslissingen nemen op basis van wat ze geleerd hebben. Zo wordt het spel spannender en realistischer en houden we het spel leuk. Ook houden we ruimte over voor uitbreidingen zoals verschillende moeilijkheidsgraden.

## Type AI Agent: Adversarial Self-Play

Door dit type agent toe te passen, kunnen we de AI trainen in een dynamische omgeving waarbij de AI zich telkens zelf uitdaagt tegen de speler (human). Dit zorgt ervoor dat de AI telkens beter presteert en minder gevoelig is aan fouten. Daarnaast ontwikkelt de AI verschillende strategieën om de race zo effectief mogelijk te vervolledigen.

## Waarom VR?

Er zijn verschillende redenen waarom het interessanter is om dit project uit te voeren in VR. Zo zorgt dit voor meer betrokkenheid van de speler. De speler kan zich meer inleven in het spel en sneller schattingen maken. Ook voor AI kan dit enorm interessant zijn, want zo leert de AI zichzelf te ontwikkeling in een realistische omgeving. Verder geeft een VR omgeving het gevoel dat de speler daadwerkelijk in de speelruimte is. Zelfs de uiterste ooghoeken van de speler zullen ingenomen worden door het spel. Al deze factoren helpen mee om het spel _immersive_ te maken.

## Interactie in de Simulatie

De speler (human) gebruikt een VR-bril met de daarbij horende controllers om het spel te besturen. De controllers maken het mogelijk dat de auto kan rijden, draaien, afremmen en achteruit rijden. De VR-bril zal het hele zicht van de speler innemen de rol van een traditionele computer- of TV-scherm overnemen.

# VR Racing: Competing against AI driven car

- Arne Van Campenhout
- Jarn Vaerewijck
- Hamzah Bhatti
- Youssef Kasmi

## Inleiding

Deze tutorial helpt de lezer om het project (van scratch) te reproduceren. Zo wordt er stap voor stap uitgelegd welke stappen er moeten ondernomen en waarom we bepaalde zakens uit te voeren. Na deze tutorial zal je een volledige AI powered project hebben en extra kennis over ML Agents.

## Methoden

### Installatie

Creeër een 3D project, importeer Race car package[^1], Race kart model[^2], Racing Arch[^4] en Quick Outline[^5]. Gebruik vervolgens volgende packages.
| (Package) naam | Versie |
|---------------------------|-------------|
| Unity versie |2022.3.19f1 |
| ML Agents | 2.0.1 |
| OpenXR Plugin | 1.9.1 |
| XR Core Utilities | 2.3.0 |
| XR Interaction Toolkit | 2.5.4 |
| XR Legacy Input Helpers | 2.1.10 |
| XR Plugin Management | 4.4.1 |

Sleep de race kart model en race car in je scene.

#### Race car modeleren

### Verloop van het spel

In onze project gaat de speler het opnemen tegen een AI. Hierbij gaat de speler, in een VR omgeving, de auto besturen doorheen de racebaan. De speler zorgt er voor dat hij de finish behaalt voor de AI dit doet. De AI wordt getraind om zo snel mogelijk de finish te behalen en krijgt rewards als hij effectief obstacles vermijd en de finish bereikt voor de speler (human).

### Gedrag (observaties, rewards)

### Beschrijving van de objecten

### One pager

### Wijken we af van de one pager?

## Resultaten

### Tensorboard

## Conclusie

###### DELETE

Youtube video[^3]
Racing Arch[^4]

###### STOP DELETE^

[^1]: [Race Car package](https://assetstore.unity.com/packages/3d/vehicles/race-car-package-141690) | 3D Vehicles | Unity Asset Store. (2021, 7 augustus). Unity Asset Store.
[^2]: [Sketchfab.](https://sketchfab.com/3d-models/race-trackkarting-track-based-on-south-garda-32c21042ba144ce9bd2822a88d5b54ec) (z.d.). Race track/Karting Track based on South Garda - Download Free 3D model by Mauro3D (@maurogsw).
[^3]: [Vanmillion Studios.](https://www.youtube.com/watch?v=jr4eb4F9PSQ) (2022, 16 april). Simple car controller in Unity 3D (Part 1- movement) | Easy Unity Tutorial 2022 [Video]. YouTube.
[^4]: [Racing Arch  | 3D model.](https://www.cgtrader.com/free-3d-models/car/racing-car/racing-arch) (z.d.). CGTrader.
[^5]: [Quick Outline](https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488) (2022, 7 maart). Quick Outline world-space outline tool by Chris Nolet
