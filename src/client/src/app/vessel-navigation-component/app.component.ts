import { Component, OnInit } from '@angular/core';
import { Chart, registerables } from 'node_modules/chart.js'
import { VesselDataService } from '../services/vessel.service'
import { Vessel } from '../models/vessel';
import { Guid } from 'guid-typescript';
import { VesselPosition } from '../models/vesselPositions';
import { ChartData } from '../models/chart-data';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [VesselDataService]
})
export class AppComponent implements OnInit {
  vesselCollection: Array<Vessel>;
  vesselsPositions: Map<Guid, Array<VesselPosition>>;
  currentVessel: Vessel;
  averageSpeed: number;
  totalDistance: number;

  constructor(private serverAPI: VesselDataService) {
    Chart.register(...registerables); // register Chart.JS

    this.vesselCollection = new Array<Vessel>();
    this.vesselsPositions = new Map<Guid, Array<VesselPosition>>();
    this.averageSpeed = 0;
    this.totalDistance = 0;
    this.loadVesselCollection();
    this.currentVessel = this.vesselCollection[0];
  }

  ngOnInit() {
    this.loadVesselCollection();
    this.currentVessel = this.vesselCollection[0];
    this.InitVesselPostionsChart();
  }

  changedVessel(event: any) {
    this.currentVessel = event.target.value as Vessel;
  }
  
  onTitleClick() {
    this.getAverageSpeedByVesselId(this.currentVessel.vesselId);
    this.getTotalDistanceByVesselId(this.currentVessel.vesselId);
    this.InitVesselPostionsChart();
  }

  // Private operations
  private loadVesselCollection() {
    try {
      this.serverAPI.getVessels().subscribe((data: Array<Vessel>) => {
        this.vesselCollection = data.sort(v => v.name ? 1 : 0);
        console.info(`Got items from server API: ${JSON.stringify(this.vesselCollection)}`);

        data.forEach(vessel => {
          try {
            this.serverAPI.getVesselPositions(vessel.vesselId).subscribe((data: Array<VesselPosition>) => {
              this.vesselsPositions.set(vessel.vesselId, data.sort(d => d.timeStamp ? 1 : 0));
              console.info(`Got items from server API: ${JSON.stringify(data)}`);
            });
          }
          catch {
            console.error('Failed to connect to server API');
          }
        });
      });
    }
    catch {
      console.error('Failed to connect to server API');
    }
  }

  private getAverageSpeedByVesselId(vesselId: Guid) {
    try {
      this.serverAPI.getAverageSpeedByVesselId(vesselId).subscribe((data: number) => {
        this.averageSpeed = data;
        console.info(`Got average speed for ${vesselId} from server API: ${data}`);
      });
    }
    catch {
      console.error('Failed to connect to server API');
    }
  }

  private getTotalDistanceByVesselId(vesselId: Guid) {
    try {
      this.serverAPI.getTotalDistanceByVesselId(vesselId).subscribe((data: number) => {
        this.totalDistance = data;
        console.info(`Got average speed for ${vesselId} from server API: ${data}`);
      });
    }
    catch {
      console.error('Failed to connect to server API');
    }
  }

  private InitVesselPostionsChart() {
    const colors = [
      'red',
      'blue',
      'green'
    ]

    var xx = new Array<ChartData>();
    this.vesselsPositions.get(this.vesselCollection[0].vesselId)?.forEach((p) => {
      xx.push(new ChartData(p.x, p.y));
    });

    var yy = new Array<ChartData>();
    this.vesselsPositions.get(this.vesselCollection[1].vesselId)?.forEach((p) => {
      yy.push(new ChartData(p.x, p.y));
    });

    var zz = new Array<ChartData>();
    this.vesselsPositions.get(this.vesselCollection[2].vesselId)?.forEach((p) => {
      zz.push(new ChartData(p.x, p.y));
    });

    new Chart("myChart", {
      type: 'scatter',
      data: {
        datasets: [
          {
            type: "line",
            borderColor: colors[0],
            label: 'vessel 1',
            data: xx.sort(x => x.x ? 1 : 0).sort(x => x.y)
          },
          {
            type: "line",
            borderColor: colors[1],
            label: 'vessel 2',
            data: yy.sort(x => x.x).sort(x => x.y)
          },
          {
            type: "line",
            borderColor: colors[2],
            label: 'vessel 3',
            data: zz.sort(x => x.x).sort(x => x.y)
          }
        ]
      },
      options: {
        scales: {
          x: {
            type: 'linear',
            position: 'bottom'
          }
        }
      }
    });
  }
}