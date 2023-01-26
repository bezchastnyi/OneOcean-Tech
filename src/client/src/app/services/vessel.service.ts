import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Vessel } from '../models/vessel';
import { Guid } from 'guid-typescript';
import { VesselPosition } from '../models/vesselPositions';

@Injectable()
export class VesselDataService {
  
  // VesselNavigationAPI
  private apiBaseUrl = "https://localhost:8080/";
  private apiVersion = "v1";

  constructor(private http: HttpClient) { }

  // RoutesCalculationApi

  getAverageSpeedByVesselId(vesselId: Guid) {
    return this.http.get<number>(`${this.apiBaseUrl}${this.apiVersion}/Vessel/AverageSpeed/${vesselId}`);
  }

  getTotalDistanceByVesselId(vesselId: Guid) {
    return this.http.get<number>(`${this.apiBaseUrl}${this.apiVersion}/Vessel/TotalDistance/${vesselId}`);
  }

  // RoutesStorageApi

  getVessels() {
    return this.http.get<Array<Vessel>>(`${this.apiBaseUrl}${this.apiVersion}/GetVessels`);
  }

  getVesselPositions(vesselId: Guid) {
    return this.http.get<Array<VesselPosition>>(`${this.apiBaseUrl}${this.apiVersion}/GetVesselPositions/${vesselId}`);
  }
}