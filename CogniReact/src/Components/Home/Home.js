import Profile from "../Profile/Profile";
import Header from "../Layouts/Header/Header";
import Navigation from "../Layouts/Navigation/Navigation";
import Settings from "../Settings/Settings";
import About from "../About/About";
import React, { useEffect, useContext} from "react";
import {Context} from "../../index";
import { useLocation, useNavigate } from 'react-router-dom';
import {observer} from "mobx-react-lite";
import './Home.css';

function Home() {
    const location = useLocation();

    const navigate = useNavigate();

    return (
        <div className="home">
            <Header></Header>
            <div className="home__bg">
            <div className="home__wrapper">
                <Navigation></Navigation>
                {location.pathname === "/profile" && <Profile />}
                {location.pathname === "/settings" && <Settings />}
                {location.pathname === "/about" && <About />}
            </div>
            </div>
        </div>
    );
};

export default observer(Home);

