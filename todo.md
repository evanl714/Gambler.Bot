### Betting Functionality
- [x] *reset stats
- [x] *login check for specific cookie and immediately proceed. seems to work for now
- [x] *custom triggers
- [x] *programmer mode error handler (kind of done i think)
- [x] *stop simulation
- [x] *update stats window on timer if bot is running but stalled
- [x] *Indicate but running status in status bar/title bar

	
### Tabs
- [ ] *Console

### Windows
- [ ] *settings import/export
- [ ] additional charts (streak occurance? roll distrobution? win/loss ratio? )
- [x] *roll verifier
- [x] *bet history


### General
- [ ] *notifications
- [ ] *Auto updates	
- [ ] *triggers for notifications (Don't think there's a lot to do here.)
- [ ] *improve programmer mode documentation
- [ ] *manual betting extra buttons (x2, 1/2, max, min, etc)
- [ ] multiple tabs
- [ ] proxy
- [ ] Telemtry (???The stats could be tremendously important but need to be really carefull about what is logged and how.???)
- [ ] CLI interface? old one should still be somewhere in the github repo, maybe compile a secondary exe for it?
- [ ] Include site name in charts and titles?
- [x] *disable/enable start/stop/resume/stoponwin based on bot state
- [x] option to run simulation without bet history

### Sites
- [ ] *wolfbet
- [ ] BCH.game

### Help windows:
- [ ] Setup wizard
- [x] *Reset to default
- [x] *source code (link to githuib repo)
- [x] *tutorials (link)
- [x] *FAQ (link)
- [x] *Conact (link)
- [x] *About

### Settings: 
- [ ] *language localization (functionality mostly in place, but strings file not yet created)
- [ ] *Errors
- [ ] Donate
- [ ] Proxy
- [ ] Telemetry
- [ ] storage strategies
- [x] *notifications (This should be done except for the grid not updating initially)
- [x] *Updates (I think this is basically done)
- [x] *Live bets
- [x] *skins
- [x] *storage: bets


### Known issues
- [ ] *Double check site settings
- [ ] *reset seed caused betting to stop
- [ ] *SiteDetails and SiteStats class are not defined before until we do a first bet
- [ ] *Add a new boolean variable that indicates if script is running in simulator
- [ ] *Balance variable is not updated in programmer mode
- [ ] *Sqlite update tables needs to be implemented
- [x] *Script conversion tool /legacy script support (untested but there for lua)
- [ ] *bot speed
- [ ] ???Specify system requirements in the readme file???
- [ ] charting line colours
- [x] *Programmer mode crash when file dialog cancelled
