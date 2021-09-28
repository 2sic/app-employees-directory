import { activeGoogleMaps } from "./google-maps/google-maps";

// so it can be called from the HTML when content re-initializes dynamically
const winAny = (window as any);
winAny.appPeopleDirectory4 ??= {};
winAny.appPeopleDirectory4.activeGoogleMaps ??= activeGoogleMaps;
