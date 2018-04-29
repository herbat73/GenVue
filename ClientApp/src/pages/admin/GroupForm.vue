<template>
    <v-card class="grey lighten-4 elevation-0">
        <v-card-text>
            <v-container fluid>
                <v-layout row>
                    <v-flex xs10 offset-xs1 lg6 offset-lg3>
                        <template>
                            <v-text-field name="name-input"
                                          label="Name"
                                          v-model="name"></v-text-field>
                            <v-subheader class="error--text" v-if="err !== null">{{ err }}</v-subheader>
                            <div class="text-md-center">
                                <v-btn color="primary" raised ripple @click.native="save_action">Save</v-btn>
                            </div>
                        </template>
                    </v-flex>
                </v-layout>
            </v-container>
        </v-card-text>
    </v-card>
</template>

<script>
    import axios from 'axios'

    export default {
        props: {
            id: Number
        },

        data () {
        return {
            name: '',
            err: ''
          }
        },

        computed: {
        },

        methods: {
            getGroupData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }
                    axios.get('/api/groups/' + this.id, config)
                        .then((res) => {
                            this.name = res.data.name
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            save_action() {
                if (this.id > 0) {
                    this.saveGroupData()
                } else {
                    this.saveNewGroup()
                }
            },

            saveGroupData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: {
                            'Authorization': 'Bearer ' + this.$store.getters.auth_data.token
                        },
                    }

                    axios.put('/api/groups/' + this.id, { id : this.id, name : this.name} , config)
                        .then(() => {
                            this.onSaveComplete()
                        }).catch((err) => {
                            reject(err)
                        })
                })                
            },

            saveNewGroup() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: {
                            'Authorization': 'Bearer ' + this.$store.getters.auth_data.token
                        },
                    }

                    axios.post('/api/groups/', { name: this.name }, config)
                        .then(() => {
                            this.onSaveComplete()
                        }).catch((err) => {
                            reject(err)
                        })
                }) 
            },

            onSaveComplete() {
                this.$router.push('/groups')
            }
        },

        created() {
            if (this.id > 0) {
                this.getGroupData()
            }
        },

    }
</script>



