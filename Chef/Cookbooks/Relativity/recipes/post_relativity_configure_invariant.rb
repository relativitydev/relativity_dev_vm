custom_log 'custom_log' do msg 'Starting configuring Invariant' end
start_time = DateTime.now
custom_log 'custom_log' do msg "recipe_start_time(#{recipe_name}): #{start_time}" end

# Make Sure Relativity Services are running
include_recipe 'Relativity::default_start_services'

# Create Processing Choice for Processing Source Location
include_recipe 'Relativity::post_relativity_configure_invariant_create_processing_choice'

# Create a Worker Manager Server code that is backwards compatible
include_recipe 'Relativity::post_relativity_configure_invariant_create_worker_manager_server'

# Add Processing To Resource Pool
include_recipe 'Relativity::post_relativity_configure_invariant_add_processing_to_default_resource_pool'

# Add Agent ResourceServer To Resource Pool
include_recipe 'Relativity::post_relativity_configure_invariant_add_agent_and_worker_servers_to_default_resource_pool'

#todo
# # Configure Worker Server for Processing
# include_recipe 'Relativity::post_relativity_configure_invariant_update_worker_server_for_processing'

end_time = DateTime.now
custom_log 'custom_log' do msg "recipe_end_Time(#{recipe_name}): #{end_time}" end
custom_log 'custom_log' do msg "recipe_duration(#{recipe_name}): #{end_time.to_time - start_time.to_time} seconds" end
custom_log 'custom_log' do msg "Finished configuring Invariant\n\n\n" end
