<template>
    <v-card color="grey lighten-4">
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
                                <v-btn color="primary"raised ripple @click.native="save_action">Save</v-btn>
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
            getData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }   
                    axios.get('/api/filecategories/' + this.id, config)
                        .then((res) => {
                            this.name = res.data.name
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            save_action() {
                if (this.id > 0) {
                    this.saveData()
                } else {
                    this.saveNew()
                }
            },

            saveData() {
                return new Promise((resolve, reject) => {
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }               
                    axios.put('/api/filecategories/' + this.id, { id : this.id, name : this.name} , config)
                        .then(() => {
                            this.onSaveComplete()
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            saveNew() {
                return new Promise((resolve, reject) => {
               
                    const config = {
                        headers: { 'Authorization': 'Bearer ' + this.$store.getters.auth_data.token },
                    }   
                    axios.post('/api/filecategories/', { name: this.name }, config)
                        .then(() => {
                            this.onSaveComplete()
                        }).catch((err) => {
                            reject(err)
                        })
                })
            },

            onSaveComplete() {
                this.$router.push('/filecategories')
            }
        },

        created() {
            if (this.id > 0) {
                this.getData()
            }
        },

    }
</script>



