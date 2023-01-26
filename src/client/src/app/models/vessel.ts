import { Guid } from 'guid-typescript';

export class Vessel {
    constructor(
        public vesselId: Guid,
        public name: string) { }
}