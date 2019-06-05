import React, { Component } from 'react';
import './Modal.css';
import axios from 'axios';
import classnames from 'classnames';

class AxiosUsers extends Component {
    state = {
        loading: true,
        list: []
    };

    

    componentDidMount = () => {
        console.log('---Hello didMount----');
        const url = '/api/users';
        axios.get(url).then(
            res => {
                this.setState({ list: res.data, loading: false });
            },
            err => {
                console.log('Error upload data', err.response.data);
            }
        );
        // setTimeout(
        //     function() {
        //         this.setState({loading: false});
        //     }
        //     .bind(this),
        //     3000
        // );
    }

    render() {
        const divStyle = {            
            width: '400px'
          };
          const imgStyle = {
            width: '100%'
          };
        const { loading, list } = this.state;
        console.log('-----render----', this.state);
        const content = list.map(item => {
            console.log('---Item render---', item);
            return  ( <div key={item.id} className="card" style={divStyle}>
                    <div className="card-body">
                        <h4 className="card-title">Email: {item.email}</h4>
                        <p className="card-text">Age: {item.age}</p>
                        <p className="card-text">Phone {item.phone}</p>
                        <p className="card-text">Description {item.description}</p>
                        <a href="#" className="btn btn-primary">See Profile</a>
                    </div>
                    <img className="card-img-bottom" src={item.image} alt="Card image" style={imgStyle} />
            </div> )
                
        });
        return (
            <div>
                <h1>
                    Axios Users Component
                </h1>
                <ul>
                    {content}
                </ul>
                {/* <div className="modal open"> */}
                <div className={classnames('modal', { 'open': loading })}>
                    <div className="position-center" >
                        <i className="fa fa-spinner fa-pulse fa-3x fa-fw"></i>
                        <span className="sr-only">Loading...</span>
                    </div>
                </div>
            </div>);
    }
}

export default AxiosUsers;