import React, { Component } from 'react'
import {Search} from 'react-bootstrap-icons'
export default class SearchBar extends Component {
    onTrigger = (event) => {
        this.props.parentCallback(event.target.location.value);
        event.preventDefault();
    }
  render() {
    return (
        <form className="input-group pb-3 pt-3" onSubmit={this.onTrigger}>
            <div className="form-outline">
                <input type="search" id="form1" name='location' className="form-control" placeholder='Search for location...' />
            </div>
            <button type="submit" className="btn btn-primary"  >
                <Search/>
            </button>
        </form>
    )
  }
}
