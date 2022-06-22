import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Department } from './components/Department';
import { Employee } from './components/Employee';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (<div>
           
            <Layout>
                <div className="App container">
                    <h3 className="d-flex justify-content-center m-3" >React JS Frontend</h3>
                </div>
                <Route exact path='/' component={Home} />
                <Route path='/employee' component={Employee} />
                <Route path='/department' component={Department} />
            </Layout>

        </div>

        );
    }
}
