import React, { Component } from 'react';
import SearchBar from './SearchBar';

export class Weather extends Component {
  static displayName = Weather.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true, location:null };
  }

  handleCallback = (childData) =>{
    this.setState({location: childData})
    this.populateWeatherData(this.state.location)
  }
 
 
  componentDidMount() {
      this.populateWeatherData(this.state.location);
  }
   
  
  renderForecastsTable (forecasts) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
                { forecasts.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
      
    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <SearchBar parentCallback = {this.handleCallback}></SearchBar>
        <p>This component demonstrates fetching data from the server.</p>
        {this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderForecastsTable(this.state.forecasts)}
        
      </div>
    );
  }

  async populateWeatherData(location) {
    console.log(location)
      if (location == null) {
          var data = "";
        await fetch("weatherforecast")
          .then((x) => x.json())
          .then((x) => {
            data = x;
            this.setState({ forecasts: data, loading: false });
          })
          .catch((x) => {
            console.log(x);
          });
        console.log(data);
      
    }
    else{ 
      let request = {                    
        place: location
    };
      const response = await fetch('weatherforecast',{
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(request)
      });
      const data = await response.json();
      this.setState({ forecasts: data, loading: false });
    }
  }
}
