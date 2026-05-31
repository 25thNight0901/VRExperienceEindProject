# Air hockey

## Makers
Jamyang Tenzin Jamyang,
Jonathan De Nies,
Lowie Casteels

## Draaiboek
1. **Initialisatie:** De gebruiker start in een virtuele arcade. Voor hen staat een airhockey tafel.
2. **De serveerfase:** De speler of AI begint met de puck voor zich te leggen en start het spel door hier tegen te slaan. De simulatie detecteert de snelheid, hoek en rotatie van de impact van de pusher op de puck
3. **De rally:**
   - De puck glijdt met snelheid over de tafel en kaatst tegen de randen
   - De AI analyseert de baan en snelheid van de puck om zijn eigen pusher strategisch te positioneren om de puck te retourneren of te verdedigen.
   - De gebruiker reageert fysiek op de inkomende puck.
4. **Scoring & Feedback**: Zodra de puck in een van de goals belandt, wordt de score bijgewerkt op een scoreboard. De puck wordt gereset voor de volgende ronde.

## De meerwaarde van AI & Agent Type
Zonder AI zou de tegenstander een voorspelbare, mechanische schijf zijn die alleen van links naar rechts schuift.
Voor het VR deel is voor ons de Adversarial Self-Play Agent de meest toepasselijke keuze. Het betekent dat de AI niet simpelweg een geprogrammeerd script volgt, maar echt begrijpt hoe hij een menselijke speler moet verslaan door patronen te herkennen.

## Waarom in VR
In VR sta je echt aan de tafel, hierdoor kan je de snelheid en de hoek van de puck beter inschatten
Je gebruikt je echte armbewegingen om de puck te blokkeren of met een hogere snelheid naar de overkant te tikken.
Door trillingen in de controllers laat je de impact van de puck echt voelen, dit zorgt voor meer immersie.

## Interacties
De VR-controller gaat gebruikt worden als de air hockey pusher.
