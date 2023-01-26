import { Guid } from 'guid-typescript';

export class VesselPosition {
    constructor(
        public vesselPositionId: Guid,
        public vesselId: Guid,
        public x: number,
        public y: number,
        public timeStamp: Date) { }
}
