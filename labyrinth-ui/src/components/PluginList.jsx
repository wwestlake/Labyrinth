import React, { useState, useEffect } from 'react';
import { Table } from 'react-bootstrap';
import { FaTrashAlt, FaInfoCircle, FaCheckCircle, FaTimesCircle } from 'react-icons/fa'; 
import { getPlugins, togglePluginStatus, deletePlugin, getPluginMetadata } from '../services/pluginService'; 
import './PluginList.css';

const PluginList = () => {
  const [plugins, setPlugins] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Fetch plugins on component mount
  useEffect(() => {
    const fetchPlugins = async () => {
      try {
        console.log("Fetching plugins...");
        const pluginData = await getPlugins(); // Fetch plugin data from the API
        console.log("Fetched plugins:", pluginData);
        setPlugins(pluginData); // Set the plugins to state
        setLoading(false); // Mark loading as false
      } catch (error) {
        console.error("Error fetching plugins:", error);
        setError('Failed to fetch plugins');
        setLoading(false);
      }
    };

    fetchPlugins();
  }, []);

  if (loading) return <div>Loading plugins...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="plugin-list">
      <h4>Available Plugins</h4>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Name</th>
            <th>Version</th>
            <th>Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {plugins.map((plugin) => (
            <tr key={plugin.id}>
              <td>{plugin.name}</td>
              <td>{plugin.version}</td>
              <td>{plugin.status}</td>
              <td>
                <div className="action-icons">
                  {plugin.status === 'enabled' ? (
                    <FaCheckCircle title="Disable Plugin" style={{ color: 'green', cursor: 'pointer' }} />
                  ) : (
                    <FaTimesCircle title="Enable Plugin" style={{ color: 'red', cursor: 'pointer' }} />
                  )}
                  <FaInfoCircle title="View Metadata" style={{ color: 'blue', cursor: 'pointer' }} />
                  <FaTrashAlt title="Delete Plugin" style={{ color: 'gray', cursor: 'pointer' }} />
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default PluginList;
